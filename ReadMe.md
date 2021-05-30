
# FunctionZero.ExpressionParserZero
A fast and very flexible parser, validator and evaluator that allows you to build, inspect, and evaluate 
infix expressions at runtime.  
Expressions can contain variables that can be rsolved against properties in your class instances.

## Overview
`ExpressionParserZero` is a `.NET Standard` library used to parse infix expressions into an `ExpressionTree` 
that can be evaluated against an `IBackingStore`.
Expressions can contain constants and variables and can make use of custom overloads and user-defined functions:
- (5 + 6) * 11.3
- ( (a + b) * (limit.top + 5) ) / 6
- 3 * MyFunc("Janet")&nbsp;&nbsp;&nbsp;&nbsp;(*with a suitable function registered*)
- 12 * "Hello World!"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(*with a suitable operator overload registered*)
   
### Features
- Compiler
  - Compile expressions once only, independently of variables
  - Does not use *reflection* or *compiler services* - safe to use in AOT environments such as iOS
  - Fully typesafe
- Variables
  - Full support for constants and runtime variables
  - **Variables can be assigned in an expression, e.g. "a = 4 + b"**
  - Supports nested variables that can be accessed by dotted notation
- DataBinding
  - Expressions can be bound to any public properties in your classes
  - If your properties support `INotifyPropertyChanged`, bindings will track all changes automatically
- Type safety
  - All `csharp` value-types are supported, including full support for `Nullable` types
  - Typecasting is supported
  - `string` and `object` reference types are supported
- Full and useful error reporting
    -  malformed expressions
    -  mismatched types
    -  missing variables or functions
    -  etc.
- Comma operator
  - e.g. "a = 4, b = a + 2"
- Extensibility:
  - New operators can be added
  - Existing operators can be aliased
  - User-defined operator overloads
  - User-defined functions can  be registered

## Quickstart
There are two primary ways to use this library.
### ExpressionBind
Assuming `this` has the public properties TestInt, Child and Child.TestInt ...
```csharp
var binding = new ExpressionBind(this, $"(TestInt + Child.TestInt) * TestInt");
this.TestInt = 5;
this.Child.TestInt = 6;
Assert.AreEqual((TestInt + Child.TestInt) * TestInt, (int)binding.Result);
```
If your properties support `INotifyPropertyChanged` you can do this and changes are automatically tracked:
```csharp
this.TestInt++;
Assert.AreEqual((TestInt + Child.TestInt) * TestInt, (int)binding.Result);
```
If your properties do not support `INotifyPropertyChanged` you can do this:
```csharp
this.TestInt++;
binding.Invalidate();       // Force a full re-evaluation next time binding.Result is read.
Assert.AreEqual((TestInt + Child.TestInt) * TestInt, (int)binding.Result);
```
### ExpressionParser
If you ~~like to make things a little harder for yourself~~ want a high level of customisation 
you can use the `ExpressionParser` directly.
```csharp
// Get a reference to an ExpressionParser:
ExpressionParser e = new ExpressionParser();

// Compile your expression:
var expressionTree = e.Parse("(TestInt + Child.TestInt) * TestInt");

// Create a backing store that can read and write properties in (for example) 'this':
var backingStore = new PocoBackingStore(this);

// Evaluate the expression using properties from the backingStore.
// The result is an OperandStack that will have a single operand. 
// Multiple operands are found if the expression uses the comma operator.
var evalResult = expressionTree.Evaluate(backingStore);

// Get the result of the evaluation:
var actualResult = (int)evalResult.Pop().GetValue();
```
## Casting
Casting follows the same rulses as `csharp`. You can cast to any type like this:
```
(<type>)<operand>
```
Where `<type>` is any type found in the `OperandType` `enum`
e.g.
```
(Long)5
(NullableInt)6
(Sbyte)-2
```
Note: At the time of writing, **function parameters must be (via a cast if necessary) the expected type.**  
E.g. the `Sin` function expects a double, so if you want the `Sin` of a `float` you must cast the float to a double 
or you will get an InvalidCastException:  
```csharp
Sin((Double)myFloat)
```

## Operators supported out of the box
|Operator|Precedence|Name|
|--|:--:|--|
|-| 12| UnaryMinus|
|+| 12| UnaryPlus|
|!| 12| UnaryNot|
|~| 12| BitwiseUnaryComplement|
|*| 11| Multiply|
|/| 11| Divide|
|%| 11| Modulo|
|+| 10| Add|
|-| 10| Subtract|
|<| 9| LessThan|
|>| 9| GreaterThan|
|>=| 9| GreaterThanOrEqual|
|<=| 9| LessThanOrEqual|
|!=| 8| NotEqual|
|==| 8| Equality|
|&| 7| BitwiseAnd|
|^| 6| BitwiseXor|
|\|| 5| BitwiseOr|
|&&| 4| LogicalAnd|
| \|\|| 3| LogicalOr|
|=|2|SetEquals|
|,|1|Comma|

## Aliases supported out of the box
|Alias|Operator|
|--|:--:|
|NOT|!|
|MOD|%|
|LT|<|
|GT|>|
|GTE|>=|
|LTE|<=|
|BAND|&| 
|XOR|^| 
|BOR|\||
|AND|&&|
|OR| \|\||

## Functions supported out of the box
|Function|Equivalent|
|--|:--:|
|Sin(double value)|Math.Sin(double value)|
|Cos(double value)|Math.Cos(double value)|
|Tan(double value)|Math.Tan(double value)|

etc.



TODO:
- Registering operator aliases
- Registering operator overloads, e.g. int + string
- Registering functions
- ExpressionTree members
- ToString on ExpressionTree.RpnTokens
- ExpressionBind events
- Reference to v3 documentation for those that like IVariableSet
- Discussion on IBackingStore, PocoBackingStore
- Introduction to PathBind
- Discuss short-circuit
- Explain comma operator and OperandStack
- Limitations:
  - object indexing not yet supported



### ExpressionTree
An `ExpressionTree` is a tree of `IToken` instances where non-leaf-nodes are an `IOperator` and leaf-nodes 
are an `IOperand`  

|Property/Method|Description|
|:--|:--|
|IEnumerable&lt;IToken> RpnTokens| Expression in reverse-polish (postfix) form|
|List&lt;ExpressionTreeNode> RootNodeList| All root nodes of the expression|
|OperandStack Evaluate(IBackingStore store| Evaluates the expression against an optional backing store|



![Image of a basic flowchart for parsing and evaluating an expression](https://raw.githubusercontent.com/Keflon/FunctionZero.ExpressionParserZero/master/Images/BasicFlowchart.png "ExpressionParserZero usage flowchart")



```csharp
public void TestExpression()
{
    TestInt = 6;
    TestLong = 41;
    var binding = new ExpressionBind(this, "(TestInt + TestLong) * TestInt");
    Assert.AreEqual((6 + 41) * 5, (int)binding.Result);

    host.Child.TestInt++;
    // If your properties raise INotifyPropertyChanged events ...
    Assert.AreEqual((7 + 41) * 5, (int)binding.Result);

    // Alternatively, if your properties do not raise INotifyPropertyChanged events
    // (or for some reason you want to force a full re-evaluation) ...
    Assert.AreEqual((7 + 41) * 5, (int)binding.Evaluate());
}
```

 
 Infix to Postfix (Reverse Polish)



[Skip to the code](#putting-it-all-together)  
[Skip to the advanced code](#advanced)





It is easy to add aliases, for example
```csharp
parser.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
```
### Parsing

The `ExpressionParser` takes an *infix* expression and produces a *postfix* `TokenList` ready for evaluation  

```csharp
var ep = new ExpressionParser();
var compiledExpression = ep.Parse("(6+2)*5");
Debug.WriteLine(compiledExpression.ToString());
```
This outputs the following *postfix* expression
```
(Long:6) (Long:2) [+] (Long:5) [*] 
```
### Evaluating
A `TokenList` can be evaluated simply by calling it's `Evaluate` method. It produces an `OperandStack` containing all the results of evaluation  
Typically this stack will contain a single `IOperand` that wraps the final result, though more complex results are possible
```csharp
var ep = new ExpressionParser();
var compiledExpression = ep.Parse("(6+2)*5");
// Evaluate ...
var resultStack = compiledExpression.Evaluate(null);
Debug.WriteLine(resultStack.ToString());
```
Output:
```
(Long:40) 
```
### Operands
Each Operand wraps a result along with the result type. To get the 'answer' to our expression "(6+2)*5":
```csharp
IOperand result = resultStack.Pop();
Debug.WriteLine(result.Type);
long answer = (long)result.GetValue();
Debug.WriteLine(answer);
```
Output:
```
Long  
40
```
As there was a single result to our expression, the result stack will now be empty

### Variables
Expressions can contain 'variables'. Compiling the expression `"(cabbages+onions)*bananas"` produces this RPN `TokenList`
```
(Variable:cabbages) (Variable:onions) [+] (Variable:bananas) [*] 
```
To evaluate this `TokenList` we need a `VariableSet` that contains the variables referenced by the expression:
```csharp
VariableSet vSet = new VariableSet();
vSet.RegisterVariable(OperandType.Double, "cabbages", 6);
vSet.RegisterVariable(OperandType.Long, "onions", 2);
vSet.RegisterVariable(OperandType.Long, "bananas", 5);
```
We can then *evaluate* or `TokenList` *against* the `VariableSet` like this:
```csharp
var resultStack = compiledExpression.Evaluate(vSet);
```
Alternatively use the `static` implementation:
```csharp
var resultStack = ExpressionEvaluator.Evaluate(compiledExpression, vSet);
```
`VariableSet` supports the following **operand** types:
- Long
- NullableLong
- Double
- NullableDouble
- String
- Bool
- NullableBool
- VSet - this is a nested VariableSet
- Object    
- Null

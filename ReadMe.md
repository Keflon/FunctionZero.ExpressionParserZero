
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
### 1. ExpressionBind
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
### Events
```csharp
public event EventHandler<EventArgs> ValueIsStale;
```
Raised when a property referenced by the expression raises an `INotifyPropertyChanged` event 
or if `Invalidate` is called directly
```csharp
public event EventHandler<ValueChangedEventArgs> ResultChanged;
```
`Result` is evaluated lazily, i.e. if `Evaluate` is called when `Result` stale.  
`Evaluate` is called by the `Result` getter.  
`Result` is stale if the binding has not yet been evaluated or if `Invalidate` has been called.  


### 2. ExpressionParser
If you ~~like to make things a little harder for yourself~~ want a high level of customisation 
you can use the `ExpressionParser` directly.
```csharp
// Get a reference to an ExpressionParser:
ExpressionParser parser = new ExpressionParser();

// Compile your expression:
var expressionTree = parser.Parse("(TestInt + Child.TestInt) * TestInt");

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
Casting follows the same rules as `csharp`. You can cast to any type like this:
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

## Operator Aliases 
The following aliases have been added to simplify writing expressions in markup, for example by 
[FunctionZero.zBind](https://www.nuget.org/packages/FunctionZero.zBind) 

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

It is easy to add aliases of your own, for example this is how the `AND` alias is registered
```csharp
parser.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
```
You can look in the *FunctionMatrices* folder to see the matrices and *vectors* for existing operators. 

## Operator overloads
To append a `bool` to a `string` register an overload like this:
```csharp
ExpressionParser parser = new ExpressionParser();

// Overload that will allow a Bool to be appended to a String
// To add a String to a Bool you'll need to add another overload
parser.RegisterOverload("+", OperandType.String, OperandType.Bool, 
    (left, right) => new Operand(OperandType.String, (string)left.GetValue() + ((bool)right.GetValue()).ToString()));
```
There is a `RegisterOperator` overload for unary operators too, should you see the need.

## Registering your own operators
You can register your own operators with the `ExpressionParser.RegisterOperator` method.  
The code for this is self-explanatory and any attempt to explain it here will only obfuscate the process, 
so take a look at the ExpressionParser constructor for examples of how to add operators and unary-operators.  
If you are serious about adding your own operators and they support many data-types you should invest some 
time looking at the `MatrixCodeGen` project and perhaps tidy it up while you're in there :)

## Pre-registered Functions
|Function|Equivalent|
|--|:--:|
|Sin(double value)|Math.Sin(double value)|
|Cos(double value)|Math.Cos(double value)|
|Tan(double value)|Math.Tan(double value)|

etc.

## User defined functions
Suppose you wanted a new function to to do a linear interpolation between two values, like this:
```csharp
double Lerp(double a, double b, double t)
{
  return a + t * (b - a);
}
```
First you will need a reference to your ExpressionParser
```csharp
var parser = ExpressionParserFactory.GetExpressionParser();
```
Then _register_ a _function_ that takes 3 parameters: 
```csharp
parser.RegisterFunction("Lerp", DoLerp, 3);
```
Finally write the `DoLerp` method referenced above, with the following signature:
```csharp
private static void DoLerp(Stack<IOperand> operandStack, IBackingStore backingStore, long paramCount)
{
    // Pop the correct number of parameters from the operands stack, ** in reverse order **
    // If an operand is a variable, it is resolved from the backing store provided
    IOperand third = OperatorActions.PopAndResolve(operandStack, backingStore);
    IOperand second = OperatorActions.PopAndResolve(operandStack, backingStore);
    IOperand first = OperatorActions.PopAndResolve(operandStack, backingStore);

    double a = (double)first.GetValue();
    double b = (double)second.GetValue();
    double t = (double)third.GetValue();

    // The result is of type double
    double result = a + t * (b - a);

    // Push the result back onto the operand stack
    operandStack.Push(new Operand(-1, OperandType.Double, result));
}
```
Don't add `Lerp` though because there's already one for you to use.

### ExpressionTrees
An `ExpressionTree` is a tree of `IToken` instances where non-leaf-nodes are an `IOperator` and leaf-nodes 
are an `IOperand`  

|Property/Method|Description|
|:--|:--|
|IEnumerable&lt;IToken> RpnTokens| Expression in reverse-polish (postfix) form|
|List&lt;ExpressionTreeNode> RootNodeList| All root nodes of the expression|
|OperandStack Evaluate(IBackingStore store| Evaluates the expression against an optional backing store|

It provides a collection of root nodes rather than a single root node, because the comma operator can be used to 
yield multiple results from an expression.

```
TIP: You can call `ToString()` on `ExpressionTree.RpnTokens` to pretty-print the expression in postfix form. 
This can be useful in debugging, for example if you are seeing rounding-errors or exceptions.
```

## VariableSet, IVariableSet ...
IVariableSet are still supported, though they are no longer required now that we can bind directly to POCO instances.  
For documentation on IVariableSet, see [v3 documentation](TODO: Put link here)

### TODO:

- Discussion on IBackingStore, PocoBackingStore
- Introduction to PathBind
- Discuss short-circuit
- Explain comma operator and OperandStack
- Replace IVariableSet in the diagram with IBackingStore
- Limitations:
  - object indexing not yet supported





![Image of a basic flowchart for parsing and evaluating an expression](https://raw.githubusercontent.com/Keflon/FunctionZero.ExpressionParserZero/master/Images/BasicFlowchart.png "ExpressionParserZero usage flowchart")


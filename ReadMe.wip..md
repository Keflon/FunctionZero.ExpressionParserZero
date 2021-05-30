# Update

### Use expressions directly in xaml!
Add the [z:Bind](https://www.nuget.org/packages/FunctionZero.zBind/) 
package and use expressions in xaml!


V4 is a major update that needs documenting. It is fully backward compatible with V3.x as documented below,
plus ...  

1. The evaluator can work directly with POCO instances, so there is no longer any need to work with `VariableSet` instances.  
2. There are new `PathBind` and `ExpressionBind` classes that can automatically re-evaluate expressions if 
`INotifyPropertyChanged` is raised on any properties in an expression.
3. All csharp value-types are supported - See the `OperandType` enum.
4. Casting is supported. Cast to any type found in OperandType, e.g. `(NullableULong)4`
5. Full support for `null` and `nullable` types.  
  
Here are a couple of teasers ...
### PathBind 

```csharp
[TestMethod]
public void TestPathBind()
{
    TestObject = new TestClass(12);

    var binding = new PathBind(this, "TestObject.TestInt");
            
    Assert.AreEqual(12, binding.Value);

    TestObject.TestInt++;
    Assert.AreEqual(13, binding.Value);
}
```
### ExpressionBind
```csharp
[TestMethod]
public void TestExpression()
{
    int count = 0;

    TestObject = new TestClass(12);

    TestInt = 5;
    TestFloat = 6.2F;

    var binding = new ExpressionBind(this, "TestObject.TestInt * (TestInt + TestFloat * 2)");

    // binding.Result is stale as soon as a property in the expression changes value.
    // It is not re-evaluated until we call the getter for binding.Result.
    binding.ValueIsStale += (sender, ea) => count++;

    Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
    Assert.AreEqual(1, count);

    TestObject.TestInt++;
    Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
    Assert.AreEqual(2, count);

    TestInt = 2;
    Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
    Assert.AreEqual(3, count);

    TestFloat = -4.6F;
    Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
    Assert.AreEqual(4, count);
}
```


  &nbsp;  
  &nbsp;  
  &nbsp;  
  






# FunctionZero.ExpressionParserZero
A fast and very flexible parser, validator and evaluator that allows you to build, inspect, and evaluate 
infix expressions at runtime.  
Expressions can contain variables that can be rsolved against properties in your class instances.

## Overview
`ExpressionParserZero` is a `.NET Standard` library used to parse infix expressions to postfix and to evaluate the postfix output 
against runtime variables  
Expressions can contain constants and variables and can make use of custom overloads and user-defined functions:
- **(5 + 6) * 11.3**
- **( (a + b) * (limit.top + 5) ) / 6**
- **3 * MyFunc("Janet")**
- **12 * "Hello World!"** (*with a suitable operator overload registered*)

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

### Features
- Compiler
  - Compile expressions once only, independently of variables
  - Does not use *reflection* or *compiler services* - safe to use in AOT environments
  - Fully typesafe
- Variables
  - Full support for constants and runtime variables
  - **Variables can be assigned in an expression, e.g. "a = 4 + b"**
  - Supports nested variables that can be accessed by dotted notation
- Full and useful error reporting
    -  malformed expressions
    -  mismatched types
    -  missing variables or functions
- Comma operator, e.g. "a = 4, b = a + 2"
- Extensibility:
  - New operators can be added
  - Existing operators can be aliased
  - User-defined operator overloads
  - User-defined functions can  be registered

### Operators supported out of the box
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
#### Events
`VariableSet` instances support `VariableAdded`, `VariableRemoved`, `VariableChanging` and 'VariableChanged` events

## Putting it all together
The following code evaluates an expression against a VariableSet:
```csharp
// parser can be a singleton ...
ExpressionParser parser = new ExpressionParser();
// Parse ...
var compiledExpression = parser.Parse("(cabbages+onions)*bananas");
Debug.WriteLine(compiledExpression.ToString());

// Variables ...
VariableSet vSet = new VariableSet();
vSet.RegisterVariable(OperandType.Double, "cabbages", 6);
vSet.RegisterVariable(OperandType.Long, "onions", 2);
vSet.RegisterVariable(OperandType.Long, "bananas", 5);

// Evaluate ...
var resultStack = compiledExpression.Evaluate(vSet);
Debug.WriteLine(TokenService.TokensAsString(resultStack));

// Result ...
IOperand result = resultStack.Pop();
Debug.WriteLine($"{result.Type}, {result.GetValue()}");
double answer = (double)result.GetValue();
Debug.WriteLine(answer);
```
Output:
```
(Variable:cabbages) (Variable:onions) [+] (Variable:bananas) [*] 
(Double:40) 
Double, 40
40
```
As the evaluator is fully typesafe and follows the same rules as csharp,  the output *type* is `Double`, as expected.

## Tips
You will likely need only one instance of `ExpressionParser` for all your parsing  
Parsing an expression into a `TokenList`can be expensive, and parsing the same expression *always* yields the *same* result, so parse once and cache the result  
A `TokenList` can be *evaluated* many times, either against different `VariableSet` objects or against the same `VariableSet` 
- If you repeatedly evaluate "a=a+1" you will see the variable 'a' climb in value


## Advanced

- Register a custom ***overload*** and a custom ***function*** with the Parser
- Use a custom `VariableFactory` to manufacture `MyVariable` instances
- Assign variables in an expression
- Use comma operator to get multiple results from a single expression
- Nest variables and use dotted notation to access them
```csharp
public class AdvancedDocumentationExample
{
    // A bag-of-horror method showing some advanced usage. Don't ever show it to my brother!
    [TestMethod]
    public void AdvancedDocumentationSample()
    {
        ///////////////////////////////////////////////
        // Create and configure the Parser object ...
        ///////////////////////////////////////////////
        ExpressionParser parser = new ExpressionParser();

        // Overload that will allow a Bool to be appended to a String
        // To add a String to a Bool you'll need to add another overload
        parser.RegisterOverload("+", OperandType.String, OperandType.Bool, 
            (left, right) => new Operand(OperandType.String, (string)left.GetValue() + ((bool)right.GetValue()).ToString()));

        // A user-defined function
        // Note (at time of writing) validation of parameter count for functions isn't robust.
        parser.RegisterFunction("StringContains", DoStringContains, 3);

        ///////////////////////////////////////////////
        // Note the comma operator - this expression yields two results each time it is evaluated
        // It calls the function 'StringContains' registered above, passing in a variable, a constant and a bool
        // It uses the overload registered above to add a bool to a string
        ///////////////////////////////////////////////
        var compiledExpression = parser.Parse(
            "text = text + child.textPhrase, 'HammerCat found: ' + (StringContains(text, 'HammerCat', true) >= 0)"
            );

        ///////////////////////////////////////////////
        // Configure a VariableSet with a custom factory 
        ///////////////////////////////////////////////
        var variableFactory = new TestVariableFactory();
        VariableSet vSet = new VariableSet(variableFactory);

        vSet.RegisterVariable(OperandType.String, "text", "GO!-> ");
        vSet.RegisterVariable(OperandType.VSet, "child", new VariableSet(variableFactory));         // VariableSet - same factory
        vSet.RegisterVariable(OperandType.String, "child.textPhrase", "Who seeks HammerCat?");      // Nested Variable

        ///////////////////////////////////////////////
        // Evaluate ...
        ///////////////////////////////////////////////
        var resultStack = compiledExpression.Evaluate(vSet);

        ///////////////////////////////////////////////
        // Get both of the results ...
        ///////////////////////////////////////////////
        // Result of "'HammerCat found: ' + StringContains(text, 'HammerCat', true) >= 0"
        var second = (string)resultStack.Pop().GetValue();

        // Result of "text = text + child.textPhrase"
        var first = (string)resultStack.Pop().GetValue();

        // Ensure the result matches the variable ...
        string text = (string)vSet.GetVariable("text").Value;
        Assert.AreEqual(first, text);

        // Show the results ...
        Debug.WriteLine($"First result is: {first}");
        Debug.WriteLine($"Second result is: {second}");
        ShowCustomVariables("", vSet);
    }

    private void ShowCustomVariables(string prefix, VariableSet vSet)
    {
        foreach (MyVariable item in vSet)
        {
            Debug.WriteLine($"The variable '{prefix}{item.VariableName}' created at {item.Timestamp} was written to {item.WriteCount} time{(item.WriteCount == 1 ? "" : "s")}");
            if (item.VariableType == OperandType.VSet)
                ShowCustomVariables(prefix + item.VariableName + ".", (VariableSet)item.Value);
        }
    }

    /// <summary>
    /// Called by the Evaluator when it encounters 'StringContains'
    /// long StringContains(string source, string subString, bool isCaseSensitive)
    /// Returns index of subString, or -1
    /// </summary>
    private void DoStringContains(Stack<IOperand> operands, IVariableStore variables, long parserPosition)
    {
        // Pop the correct number of parameters from the operands stack, ** in reverse order **
        // If an operand is a variable, it is resolved from the variables provided
        IOperand third = OperatorActions.PopAndResolve(operands, variables);
        IOperand second = OperatorActions.PopAndResolve(operands, variables);
        IOperand first = OperatorActions.PopAndResolve(operands, variables);

        string strSource = (string)first.GetValue();
        string strSubstring = (string)second.GetValue();
        StringComparison comparison = (bool)third.GetValue() == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        long result = strSource.IndexOf(strSubstring, comparison);

        operands.Push(new Operand(-1, OperandType.Long, result));
    }

    /// <summary>
    /// A basic IVariableFactory
    /// </summary>
    public class TestVariableFactory : IVariableFactory
    {
        public Variable CreateVariable(string name, OperandType type, object defaultValue, object state)
        {
            return new MyVariable(name, type, defaultValue, DateTime.UtcNow.ToString());
        }
    }

    /// <summary>
    /// A specialisation of a Variable for the TestVariableFactory to manufacture
    /// </summary>
    internal class MyVariable : Variable
    {
        public long WriteCount { get; private set; } = 1;
        public string Timestamp { get; }

        public MyVariable(string variableName, OperandType variableType, object startingValue, string timestamp) : base(variableName, variableType, startingValue, "Hello!")
        {
            this.VariableChanged += MyVariable_VariableChanged;
            Timestamp = timestamp;
        }

        private void MyVariable_VariableChanged(object sender, VariableChangedEventArgs e)
        {
            WriteCount++;
        }
    }
}
```
Output:
```
First result is: GO!-> Who seeks HammerCat?
Second result is: HammerCat found: True
The variable 'text' created at 23/04/2020 19:46:28 was written to 2 times
The variable 'child' created at 23/04/2020 19:46:28 was written to 1 time
The variable 'child.textPhrase' created at 23/04/2020 19:46:28 was written to 1 time
```

# Further documentation

For the time being I suggest you poke around the unit tests. Please get in touch if you have questions or requests.










# *** DOCUMENTATION WORK IN PROGRESS ***

# FunctionZero.ExpressionParserZero
A fast and very flexible Infix to Postfix (Reverse Polish) parser, validator and evaluator.

![Image of a basic flowchart for parsing and evaluating an expression](https://raw.githubusercontent.com/Keflon/FunctionZero.ExpressionParserZero/master/Images/BasicFlowchart.png "ExpressionParserZero usage flowchart")

## Quickstart

`ExpressionParserZero` uses three primary class instances

### To evaluate (5+2)

```csharp
        public void AddTwoLongs()
        {
            ExpressionParser parser = new ExpressionParser();
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            var compiledExpression = parser.Parse("5+2");
            Debug.WriteLine("Compiled expression: "+TokenService.TokensAsString(compiledExpression));

            var evaluatedResult = evaluator.Evaluate(compiledExpression, null);
            Debug.WriteLine("Results Stack: "+TokenService.TokensAsString(evaluatedResult));

            long answer = (long)evaluatedResult.Pop().GetValue();
            Debug.WriteLine(answer);
        }
        // Output: [Operand:5][Operand:2][Operator:+]
        // Output: [Operand:7]
```


dzfgd
- Rich syntax checking
- Type-safe variable-sets with full cutomisation
- Operator overloads
- Function registration
- Pre-compiling expressions independently of variables

Supports the following operand types:
- Long
- NullableLong
- Double
- NullableDouble
- String
- Variable
- Bool
- NullableBool
- VSet
- Object    
- Null




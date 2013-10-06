﻿namespace Rosalia.TaskLib.AssemblyInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Nustache.Core;
    using Rosalia.Core.Context;
    using Rosalia.Core.FileSystem;
    using Rosalia.Core.Fluent;
    using Rosalia.Core.Tasks;

    public class GenerateAssemblyInfo<T> : AbstractLeafTask<T>
    {
        private const string Template =
@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

{{#Namespaces}}
using {{Name}};
{{/Namespaces}}

{{#Attributes}}
[assembly: {{Name}}({{{Arguments}}})]
{{/Attributes}}
";

        private readonly IList<Expression<Action<Attribute>>> _attributeExpressions = new List<Expression<Action<Attribute>>>();
        private IFile _destination;

        public GenerateAssemblyInfo<T> WithAttribute(Expression<Action<Attribute>> attribute)
        {
            _attributeExpressions.Add(attribute);
            return this;
        }

        public GenerateAssemblyInfo<T> ToFile(string destination)
        {
            _destination = new DefaultFile(destination);
            return this;
        }

        public GenerateAssemblyInfo<T> ToFile(IFile destination)
        {
            _destination = destination;
            return this;
        }

        protected override void Execute(ResultBuilder resultBuilder, TaskContext<T> context)
        {
            if (_destination == null)
            {
                resultBuilder.AddError("Destinatin file is not set");
                resultBuilder.Fail();
                return;
            }

            var model = new Model
            {
                Attributes = new List<AttributeInfo>(),
                Namespaces = new List<Namespace>()
            };

            foreach (var attributeExpression in _attributeExpressions)
            {
                var body = (NewExpression)attributeExpression.Body;
                var attributeInfo = new AttributeInfo();

                var namespaceName = body.Type.Namespace;

                if (model.Namespaces.All(n => n.Name != namespaceName))
                {
                    model.Namespaces.Add(new Namespace
                    {
                        Name = namespaceName
                    });
                }

                attributeInfo.Name = body.Type.Name;
                attributeInfo.Arguments = string.Join(", ", GetArgumetValues(body));

                model.Attributes.Add(attributeInfo);
            }

            var result = Render.StringToString(Template, model);

            context.FileSystem.WriteStringToFile(result, _destination);
            resultBuilder.AddInfo(result);
        }

        private IEnumerable<string> GetArgumetValues(NewExpression body)
        {
            foreach (var expression in body.Arguments)
            {
                var rawValue = Expression.Lambda(expression).Compile().DynamicInvoke().ToString();
                yield return Expression.Constant(rawValue).ToString();
            }
        }

        internal class Namespace
        {
            public string Name { get; set; }
        }

        internal class AttributeInfo
        {
            public string Name { get; set; }

            public string Arguments { get; set; }
        }

        internal class Model
        {
            public IList<Namespace> Namespaces { get; set; }

            public IList<AttributeInfo> Attributes { get; set; }
        }
    }
}
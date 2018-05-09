﻿using System.Collections.Generic;
using System.Diagnostics;
using BoDi;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.Generator.CodeDom;
using TechTalk.SpecFlow.Generator.UnitTestConverter;
using TechTalk.SpecFlow.Parser;

namespace TechTalk.SpecFlow.Specs.Generator.SpecFlowPlugin
{
    public class Combination
    {
        public string ProjectFormat { get; set; }
        public string TargetFramework { get; set; }
        public string ProgrammingLanguage { get; set; }
    }

    internal class MultiFeatureGeneratorProvider : IFeatureGeneratorProvider
    {
        private readonly List<Combination> _combination = new List<Combination>()
        {
            new Combination(){ProgrammingLanguage = "CSharp", ProjectFormat = "Old", TargetFramework = "Net452"},
            new Combination(){ProgrammingLanguage = "CSharp", ProjectFormat = "New", TargetFramework = "Net452"}
        };

        private readonly MultiFeatureGenerator _multiFeatureGenerator;

        public MultiFeatureGeneratorProvider(IObjectContainer container)
        {
            var featureGenerators = new List<KeyValuePair<Combination, IFeatureGenerator>>();

            foreach (var combination in _combination)
            {
                var combinationFeatureGenerator = new CombinationFeatureGenerator(container.Resolve<CodeDomHelper>(), container.Resolve<SpecFlowConfiguration>(), container.Resolve<IDecoratorRegistry>(), combination);
                featureGenerators.Add(new KeyValuePair<Combination, IFeatureGenerator>(combination, combinationFeatureGenerator));
            }

            
            _multiFeatureGenerator = new MultiFeatureGenerator(featureGenerators);
        }

        
        public bool CanGenerate(SpecFlowDocument specFlowDocument)
        {
            return true;
        }

        public IFeatureGenerator CreateGenerator(SpecFlowDocument specFlowDocument)
        {
            return _multiFeatureGenerator;
        }

        public int Priority => PriorityValues.Normal;
    }
}
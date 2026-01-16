public class PackingRulesHillDescending : HillDescending<PackingRules>
{
    public PackingRulesHillDescending(IReadOnlyList<PackingRules> initialPopulation, IFitnessEvaluator<PackingRules> fitnessEvaluator, HillDescendingSetting setting, IIterationStatistics<PackingRules>? data = null)
        : base(initialPopulation, fitnessEvaluator, new PackingRulesMutation(setting.AverageElementsChanged), setting, data)
    { }
}
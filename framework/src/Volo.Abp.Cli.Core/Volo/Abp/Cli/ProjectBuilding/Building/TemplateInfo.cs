﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates;

namespace Volo.Abp.Cli.ProjectBuilding.Building;

public abstract class TemplateInfo
{
    [NotNull]
    public string Name { get; }

    public DatabaseProvider DefaultDatabaseProvider { get; }

    public UiFramework DefaultUiFramework { get; }

    [CanBeNull]
    public string DocumentUrl { get; set; }

    protected TemplateInfo(
        [NotNull] string name,
        DatabaseProvider defaultDatabaseProvider = DatabaseProvider.NotSpecified,
        UiFramework defaultUiFramework = UiFramework.NotSpecified)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        DefaultDatabaseProvider = defaultDatabaseProvider;
        DefaultUiFramework = defaultUiFramework;
    }

    public virtual IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = new List<ProjectBuildPipelineStep>();
        ConfigureCheckPreRequirements(context, steps);
        return steps;
    }

    protected void ConfigureCheckPreRequirements(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new CheckRedisPreRequirements());
    }

    public bool IsPro()
    {
        return Name.EndsWith("-pro", StringComparison.OrdinalIgnoreCase);
    }
    
    public bool IsNoLayer()
    {
        return Name is AppNoLayersTemplate.TemplateName or AppNoLayersProTemplate.TemplateName;
    }
}

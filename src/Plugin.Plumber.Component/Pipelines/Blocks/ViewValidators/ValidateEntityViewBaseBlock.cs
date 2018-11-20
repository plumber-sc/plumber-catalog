using Plugin.Plumber.Component.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Plumber.Component.Pipelines.Blocks.ViewValidators
{
    public abstract class ValidateEntityViewBaseBlock<EntityType> : PipelineBlock<EntityViewConditionsArgument, EntityViewConditionsArgument, CommercePipelineExecutionContext>
    {
        public override async Task<EntityViewConditionsArgument> Run(EntityViewConditionsArgument arg, CommercePipelineExecutionContext context)
        {
            arg.ValidateEntity(ent => ent is EntityType);
            arg.ValidateDisplayView(viewName => viewName.Equals(GetMasterViewName(context), StringComparison.OrdinalIgnoreCase));
            arg.ValidateEditView(action => action.StartsWith("Edit-", StringComparison.OrdinalIgnoreCase));

            return await Task.FromResult(arg);
        }

        protected abstract string GetMasterViewName(CommercePipelineExecutionContext context);

    }
}
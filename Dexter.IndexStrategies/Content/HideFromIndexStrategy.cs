namespace Dexter.IndexStrategies.Content
{
    using Dexter.Core.Interfaces;
    using Dexter.Core.Models.IndexStrategy;

    public class HideFromIndexStrategy : IContentIndexStrategy
    {
        protected const string hideFromIndex = "hideFromIndex";

        public void Execute(IndexContentEvent e)
        {
            if (!e.Content.HasProperty(hideFromIndex))
            {
                return;
            }

            e.Cancel = e.Content.GetValue<bool>(hideFromIndex);
        }
    }
}

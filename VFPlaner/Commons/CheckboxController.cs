using DevExpress.XtraGrid.Localization;
namespace Controller.Grid
{

    public class CustomGridLocalizer : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            if (id == GridStringId.CheckboxSelectorColumnCaption)
            {
                return "CustomCaption";
            }
            return base.GetLocalizedString(id);
        }
    }
}
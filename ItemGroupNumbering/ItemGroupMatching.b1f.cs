using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace ItemGroupNumbering
{
    [FormAttribute("ItemGroupNumbering.ItemGroupMatching", "ItemGroupMatching.b1f")]
    class ItemGroupMatching : UserFormBase
    {
        public ItemGroupMatching()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_0").Specific));
            this.Grid0.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid0_ClickAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);

        }

        private SAPbouiCOM.Grid Grid0;

        private void Refresh()
        {
            Grid0.DataTable.ExecuteQuery(DiManager.QueryHanaTransalte($"select Series, SeriesName, [@RSM_ITGR].U_ItmsGrpCod as [Group Code], [@RSM_ITGR].U_ItmsGrpNam as [Group Name] from NNM1 left join[@RSM_ITGR] on NNM1.Series = [@RSM_ITGR].U_Series where ObjectCode = 4"));
        }

        private void OnCustomInitialize()
        {
            Refresh();
        }

        private void Grid0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            Grid0.Rows.SelectedRows.Clear();
            Grid0.Rows.SelectedRows.Add(pVal.Row);
        }

        private SAPbouiCOM.Button Button0;

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var selectedRow = Grid0.Rows.SelectedRows;
            if (selectedRow.Count < 1)
            {
                return;
            }
            var rowIndex = selectedRow.Item(0, BoOrderType.ot_RowOrder);
            string seriesCode = Grid0.DataTable.GetValue("Series", rowIndex).ToString();
            string seriesName = Grid0.DataTable.GetValue("SeriesName", rowIndex).ToString();
            ItemGroups itmGroups = new ItemGroups(seriesCode, seriesName);
            itmGroups.Show();
        }

        private void Form_ActivateAfter(SBOItemEventArg pVal)
        {
            Refresh();
        }
    }
}

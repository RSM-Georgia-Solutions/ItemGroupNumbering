using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SAPbouiCOM.Framework;

namespace ItemGroupNumbering
{
    [FormAttribute("ItemGroupNumbering.ItemGroups", "ItemGroups.b1f")]
    class ItemGroups : UserFormBase
    {
        private string _seriesCode;
        private string _seriesName;
        public ItemGroups(string seriesCode, string seriesName)
        {
            _seriesCode = seriesCode;
            _seriesName = seriesName;
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
        }

        private SAPbouiCOM.Grid Grid0;

        private void OnCustomInitialize()
        {
            Refresh();
        }

        private void Refresh()
        {
            Grid0.DataTable.ExecuteQuery(DiManager.QueryHanaTransalte($"select ItmsGrpCod as [ჯგუფის კოდი],ItmsGrpNam as [ჯგუფის დასახელება]  from OITB WHERE ItmsGrpCod not in (select U_ItmsGrpCod from [@RSM_itgr])"));
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
                string groupCode = Grid0.DataTable.GetValue("ჯგუფის კოდი", rowIndex).ToString();
                string groupName = Grid0.DataTable.GetValue("ჯგუფის დასახელება", rowIndex).ToString();



            DiManager.Recordset.DoQuery(DiManager.QueryHanaTransalte($"SELECT * FROM [@RSM_ITGR] WHERE U_Series = {_seriesCode}"));
            if (DiManager.Recordset.EoF)
            {
                DiManager.Recordset.DoQuery(DiManager.QueryHanaTransalte($"INSERT INTO [@RSM_ITGR] (U_Series, U_SeriesName, U_ItmsGrpCod, U_ItmsGrpNam) VALUES (N'{_seriesCode}', N'{_seriesName}', N'{groupCode}', N'{groupName}')"));
            }
            else
            {
                DiManager.Recordset.DoQuery(DiManager.QueryHanaTransalte($"UPDATE [@RSM_ITGR] Set U_Series = N'{_seriesCode}' , U_SeriesName = N'{_seriesName}', U_ItmsGrpCod = N'{groupCode}', U_ItmsGrpNam = N'{groupName}' WHERE U_Series = {_seriesCode}"));
            }

            SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm.Close();



        }
    }
}

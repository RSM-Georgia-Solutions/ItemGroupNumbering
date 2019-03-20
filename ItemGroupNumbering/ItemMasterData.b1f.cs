using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using Application = SAPbouiCOM.Framework.Application;

namespace ItemGroupNumbering
{
    [FormAttribute("150", "ItemMasterData.b1f")]
    class ItemMasterData : SystemFormBase
    {
        public ItemMasterData()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button1_PressedBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Button Button1;

        private void Button1_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.FormMode != 3)
            {
                return;
            }
            var ItemMasterData = SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            string series = ((ComboBox)ItemMasterData.Items.Item("1320002059").Specific).Value;
            string group = ((ComboBox)ItemMasterData.Items.Item("39").Specific).Value;

            DiManager.Recordset.DoQuery(DiManager.QueryHanaTransalte($"SELECT * FROM [@RSM_ITGR] WHERE U_Series = N'{series.Trim()}' AND U_ItmsGrpCod = N'{group.Trim()}'"));
            if (DiManager.Recordset.EoF)
            {
                Application.SBO_Application.SetStatusBarMessage("დანომვრა არ შეესაბამება საქონლის ჯგუფს",
                    BoMessageTime.bmt_Short, true);
                BubbleEvent = false;
            }

        }

        private void OnCustomInitialize()
        {

        }
    }
}

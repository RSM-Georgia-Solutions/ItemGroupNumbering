using System;
using System.Collections.Generic;
using System.Xml;
using SAPbobsCOM;
using SAPbouiCOM.Framework;

namespace ItemGroupNumbering
{
    [FormAttribute("ItemGroupNumbering.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Button Button0;
        private DiManager diManager;
        private void OnCustomInitialize()
        {
             diManager = new DiManager();

        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            diManager.CreateTable("RSM_ITGR", BoUTBTableType.bott_NoObjectAutoIncrement);
            diManager.AddField("RSM_ITGR", "Series", "Series", BoFieldTypes.db_Alpha, 50, true);
            diManager.AddField("RSM_ITGR", "SeriesName", "Series Name", BoFieldTypes.db_Alpha, 250, false);
            diManager.AddField("RSM_ITGR", "ItmsGrpCod", "Itmems Group Code", BoFieldTypes.db_Alpha, 50, true);
            diManager.AddField("RSM_ITGR", "ItmsGrpNam", "Itmems Group Code", BoFieldTypes.db_Alpha, 250, false);
            diManager.AddKey("RSM_ITGR", "xz", "Series", BoYesNoEnum.tYES, "ItmsGrpCod");
        }
    }
}
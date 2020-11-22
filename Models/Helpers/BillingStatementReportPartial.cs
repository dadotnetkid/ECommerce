using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;

namespace Helpers
{
    public partial class BillingStatementReport
    {
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;

        public BillingStatementReport(List<BillingStatementViewModel> model)
        {
            DataSource = model;
            var item = model?.FirstOrDefault();
            if (item != null)
            {
                if (!item.TransactionFacilities.Any())
                    this.groupfacilities.Visible = false;
                if (item.TransactionVehicles.Any())
                    this.groupVehicles.Visible = false;
                if (!item.TaxOnExcessSAGVolume.Any())
                    this.groupExcessSag.Visible = false;
                if (item.Transactions.TransactionTypes.TransactionType .Contains( "Additional"))
                    this.groupNotary.Visible = false;
                if (!item.GovernorsBusinessPermitFee.Any())
                    this.groupGovBusiness.Visible = false;
                if (!item.GovernorsAccreditationFees.Any())
                    this.detailAccre.Visible = false;
            }
        }

        private void InitializeComponent()
        {
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Name = "detailBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // BillingStatementReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.Version = "18.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}



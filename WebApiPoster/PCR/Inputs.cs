using System.Collections.Generic;
using System.Linq;

namespace WebApiPoster.PCR
    {
    public class Inputs : PcrBase
        {
        public string value { get; set; }
        public string pcr_id { get; set; }
        public string input_id { get; set; }
        public string _ButtonID { get; set; }
        //        public All_Buttons button_object { get; set; }

        public Inputs() { TableName = "pcr_inputs"; }
        public Inputs(string id, string SearchField = "id")
            : base(id, "pcr_inputs", SearchField)
            {
            //  if (!string.IsNullOrEmpty(button_id)) button_object = new All_Buttons(button_id);
            }
        public Inputs(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }
            }

        public void HandleRecord(int InsertUpdate = 0)
            {
           
            ValidateFields();
            this.InsertUpdateAction(InsertUpdate);

            }

        public void ValidateFields()
            {
            Utilities.ValidateField("pcr", pcr_id);
            //Utilities.ValidateField("All_Buttons", input_id );

            }

        }
    }

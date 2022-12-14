// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseControlSystem.Model.NAV
{
    /// <summary>
    /// Microsoft Dynamics NAV - Bin Content Record
    /// </summary>
    public class BinContent 
    {
        public string LocationCode { get; set; } = "";
        public string ZoneCode { get; set; } = "";
        public string BinCode { get; set; } = "";
        public string BinType { get; set; } = "";
        public string ItemNo { get; set; } = "";
        public string VariantCode { get; set; } = "";
        public string Description { get; set; } = "";
        public int BlockMovement { get; set; }
        public string UnitofMeasureCode { get; set; } = "";
        public decimal Quantity { get; set; }
        public decimal QuantityBase { get; set; }
        public decimal PickQty { get; set; }
        public decimal NegAdjmtQty { get; set; }
        public decimal PutawayQty { get; set; }
        public decimal PosAdjmtQty { get; set; }
        public string ImageURL { get; set; }
    }
}

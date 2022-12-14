// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
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
using System.Text;

namespace WarehouseControlSystem.Model.NAV
{
    public class Indicator
    {
        public string Header { get; set; } = "";
        public string Description { get; set; } = "";
        public string Value { get; set; } = "";
        public string ValueColor { get; set; } = "";
        public int Position { get; set; }
        public string URL { get; set; } = "";

        public string ID { get; set; } = "";

        public string Parameters { get; set; } = "";
    }
}

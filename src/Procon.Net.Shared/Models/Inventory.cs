﻿using System;
using System.Collections.Generic;

namespace Procon.Net.Shared.Models {
    [Serializable]
    public sealed class Inventory : NetworkModel {

        public Inventory() {
            this.Now.Items = new List<Item>();
        }
    }
}

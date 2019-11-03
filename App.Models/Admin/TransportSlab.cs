﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Models.Admin
{
	class TransportSlab
	{
		public int ExpensesTypeid { get; set; }
		[JsonProperty("slab")]
		public string Slab { get; set; }
		[JsonProperty("amount")]
		public decimal Amount { get; set; }
	}
}
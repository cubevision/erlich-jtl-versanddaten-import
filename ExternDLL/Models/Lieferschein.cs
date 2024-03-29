﻿using System;

namespace JTLVersandImport.Models
{
    public class Lieferschein
    {
        public int ID { get; set; }
        public int BestellId { get; set; }
        public String LieferscheinNummer { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(ID)} = {ID}, {nameof(BestellId)}={BestellId}, {nameof(LieferscheinNummer)}={LieferscheinNummer}}}";
        }
    }
}

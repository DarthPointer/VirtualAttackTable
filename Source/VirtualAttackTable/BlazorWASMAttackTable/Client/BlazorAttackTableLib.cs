namespace BlazorWASMAttackTable.Client
{
    public static class BlazorAttackTableLib
    {
        public static string GetShipImagePath(string typeName)
        {
            return $"ShipAssets/{typeName}.png";
        }

        public static string CustomUnitValueFormat(float value)
        {
            float abs = float.Abs(value);

            if (abs >= 1000) return value.ToString("f0");

            if (abs >= 100) return value.ToString("f1");

            if (abs >= 10) return value.ToString("f2");

            if (abs >= 1) return value.ToString("f3");

            if (abs >= 0.1) return value.ToString("f4");

            return value.ToString("f5");
        }
    }
}


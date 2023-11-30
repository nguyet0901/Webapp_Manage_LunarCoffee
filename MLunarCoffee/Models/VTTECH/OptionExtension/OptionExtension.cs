
public class OptionExtension
{
    public OptionValue Before_16 { get; set; }
    public OptionValue PreHistory { get; set; }
    public OptionValue TimeInvoice { get; set; }
    public OptionValue CustomerGroup { get; set; }
    public OptionValue ValidProfile { get; set; }
    public OptionValue InputTeethChart { get; set; }
}

public class OptionValue
{
    public int Value { get; set; } = 0;
    public int TypeRule { get; set; } = 0;
}


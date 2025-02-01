using Abstracts;

namespace MenuSystem;

public class MenuItem : AbstractMenuItem
{
    private readonly string _title = default!;
    private readonly string _shortcut = default!;

    public Func<string> MenuItemAction { get; init; } = default!;

    //TODO: Validate that Shortcut isn't R or E or M (aka menu returns)
    public string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ApplicationException("Title can't be empty");
            }

            _title = value;
        }
    }

    public string Shortcut
    {
        get => _shortcut;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ApplicationException("Shortcut can't be empty");
            }

            _shortcut = value;
        }
    }

    public override string ToString()
    {
        return $"{Shortcut}) {Title}";
    }
}
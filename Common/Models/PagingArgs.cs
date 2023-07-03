namespace Common.Models;

public class PagingArgs
{
    private int _limit = 20;

    public static PagingArgs NoPaging => new PagingArgs { UsePaging = false };

    public static PagingArgs Default => new PagingArgs { UsePaging = true, Limit = 20, Offset = 0 };

    public static PagingArgs FirstItem => new PagingArgs { UsePaging = true, Limit = 1, Offset = 0 };

    public int Offset { get; set; }

    public int Limit
    {
        get => this._limit;

        set
        {
            if (value == 0)
            {
                value = 20;
            }

            this._limit = value;
        }
    }

    public bool UsePaging { get; set; }
}
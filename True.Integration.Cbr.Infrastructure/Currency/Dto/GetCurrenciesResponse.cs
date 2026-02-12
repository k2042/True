using System;
using System.Collections.Generic;
using System.Text;

namespace True.Integration.Cbr.Infrastructure.Currency.Dto
{
    public record GetCurrenciesResponse(ValCurs? Data = null, string? Error = null);
}

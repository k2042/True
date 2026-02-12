using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace True.Integration.Cbr.Features.Currencies.Commands
{
    public record LoadCurrenciesCommandResult(int Rows, string? Error = null);
}

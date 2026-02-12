using System;
using System.Collections.Generic;
using System.Text;

namespace True.Gateway.Infrastructure.Finance.Abstractions
{
    public record FavoritesDto(IEnumerable<string> Ids);
}

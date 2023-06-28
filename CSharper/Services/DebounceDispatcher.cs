using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public class DebounceDispatcher
    {
        private int _quertVersion = 0;
        public async Task Debounce(TimeSpan timeOut, Func<Task> func)
        {
            _quertVersion++;
            var savedVerion = _quertVersion;

            await Task.Delay(timeOut);

            if (savedVerion == _quertVersion)
            {
                await func();
            }
        }
    }
}

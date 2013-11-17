/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System.Collections.Generic;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class CountryQueue
    {
        private readonly object lockObject = new object();
        private Queue<CountryUser> _countryQueue = new Queue<CountryUser>();

        public void Clear()
        {
            lock (lockObject)
            {
                _countryQueue.Clear();
            }
        }

        public void Add(CountryUser cuser)
        {
            lock (lockObject)
            {
                _countryQueue.Enqueue(cuser);
            }
        }

        public CountryUser Get()
        {
            if (_countryQueue.Count > 0)
            {
                lock (lockObject)
                {
                    if (_countryQueue.Count > 0)
                    {
                        return _countryQueue.Dequeue();
                    }
                }
            }
            return null;
        }

        public CountryUser Peek()
        {
            if (_countryQueue.Count > 0)
            {
                lock (lockObject)
                {
                    if (_countryQueue.Count > 0)
                    {
                        return _countryQueue.Peek();
                    }
                }
            }
            return null;
        }

        public int Count { get { return _countryQueue.Count; } }

    }
}
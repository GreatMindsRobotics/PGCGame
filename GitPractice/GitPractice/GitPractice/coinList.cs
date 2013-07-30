using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitPractice
{
    class coinList
    {
        List<BaseEnemy> list;

        public coinList(int amt)
        {
            list = new List<BaseEnemy>();
            for (int i = 0; i < amt; i++)
            {

            }
        }

        public coinList()
        {
            list = new List<BaseEnemy>();
            for (int i = 0; i < 10; i++)
            {

            }
        }
    }
}

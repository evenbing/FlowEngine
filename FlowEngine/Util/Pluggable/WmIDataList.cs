using FlowEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Util.Pluggable
{
    interface WmIDataList
    {
          void putItemAt(IData paramIData, int paramInt);

          IData getItemAt(int paramInt);

          void putItems(IData[] paramArrayOfIData);

          IData[] getItems();

          void deleteItemAt(int paramInt);

          int getSize();
    }
}

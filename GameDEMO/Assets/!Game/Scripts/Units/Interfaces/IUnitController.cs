using System.Collections.Generic;
using Colyseus.Schema;

namespace _Game.Scripts.Units.Interfaces
{
    public interface IUnitController
    {
        public void OnChange(List<DataChange> changes);
    }
}
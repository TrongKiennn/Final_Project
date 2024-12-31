using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Rank
{
    public Model.Rank GetRank(string rankName);
    public void UpdateRankSetting(Model.Rank rank,decimal pointPerDollar);

}



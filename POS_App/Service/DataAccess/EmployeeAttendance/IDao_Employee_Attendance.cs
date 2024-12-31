using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Employee_Attendance
{
   
    Tuple<int, decimal> GetEmployeeAttendence(int user_id);
    public employeeAttendance FindEmployeeAttendanceByUser_IdToday(int user_id);
    public void CreateEmployeeAttendance(int user_id);
    public void UpdateCheckInTime(int user_id);
    public void UpdateCheckOutTime(int user_id);

}



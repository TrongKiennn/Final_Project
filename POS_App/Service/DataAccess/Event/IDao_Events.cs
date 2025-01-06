using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Events
{

    public Task<List<Event>> GetEventsWithinTimeFrameAsync(DateTime from, DateTime to) ;
    public Task UpdateEventStatus(int eventId, string status);
    public List<Event> GetAllEvent();
    public void AddEvent(Event Event); 
    public Event GetEventById(int id);
}

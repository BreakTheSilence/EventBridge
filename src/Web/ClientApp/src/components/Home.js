import React, { useState, useEffect } from 'react';
import {useNavigate} from "react-router-dom";
import {EventsClient} from "../web-api-client.ts";

function Home() {
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();


  useEffect(() => {
    const populateEvents = async () => {
      let eventsClient = new EventsClient();
      const data = await eventsClient.getEventsWithPagination(1, 20);
      setEvents(data.items);
      setLoading(false);
    };
    populateEvents();
  }, []);

  const handleDeleteEvent = async (eventId) => {
    let eventsClient = new EventsClient();
    await eventsClient.deleteEvent(eventId)
    const updatedEvents = events.filter(event => event.id !== eventId);
    setEvents(updatedEvents);
  };
  
  const handleOpenDetails = async (eventId) => {
    navigate(`/event-details/${eventId}`)
  };

  const renderEventsTable = (events) => (
    <div>
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Date</th>
          <th></th> {}
          <th></th> {}
        </tr>
        </thead>
        <tbody>
        {events.map(event => (
          <tr key={event.id}>
            <td>{event.id}</td>
            <td>{event.name}</td>
            <td>{event.date.toLocaleDateString()}</td>
            <td>
              <button onClick={() => handleOpenDetails(event.id)}>Participants</button>
            </td>
            <td>
              <button onClick={() => handleDeleteEvent(event.id)}>Delete</button>
            </td>
          </tr>
        ))}
        </tbody>
      </table>
    </div>
  );

  let contents = loading
    ? <p><em>Loading...</em></p>
    : renderEventsTable(events);

  return (
    <div>
      <h1 id="tableLabel">Events</h1>
      <p>All events list.</p>
      {contents}
    </div>
  );
}
export default Home;
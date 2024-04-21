import React, { useState, useEffect } from 'react';
import { useNavigate } from "react-router-dom";
import { EventsClient } from "../web-api-client.ts";
import Stack from "@mui/material/Stack";

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
    await eventsClient.deleteEvent(eventId);
    const updatedEvents = events.filter(event => event.id !== eventId);
    setEvents(updatedEvents);
  };

  const handleOpenDetails = async (eventId) => {
    navigate(`/event-details/${eventId}`);
  };

  const renderEventsTable = (events, includeDelete = true) => (
    <div>
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
        <tr>
          <th>Name</th>
          <th>Date</th>
          <th>Details</th>
          {includeDelete && <th>Delete</th>}
        </tr>
        </thead>
        <tbody>
        {events.map(event => (
          <tr key={event.id}>
            <td>{event.name}</td>
            <td>{event.date.toLocaleDateString()}</td>
            <td>
              <button onClick={() => handleOpenDetails(event.id)}>Details</button>
            </td>
            {includeDelete && (
              <td>
                <button onClick={() => handleDeleteEvent(event.id)}>Delete</button>
              </td>
            )}
          </tr>
        ))}
        </tbody>
      </table>
    </div>
  );

  const now = new Date();
  const pastEvents = events.filter(event => new Date(event.date) < now);
  const futureEvents = events.filter(event => new Date(event.date) >= now);

  return (
    <div>
      <h1 id="tableLabel">Events</h1>
      <p>All events list.</p>
      {loading ? <p><em>Loading...</em></p> : (
        <>
        <Stack spacing={20} direction="row">
          <Stack direction={"column"}>
            <h2>Future Events</h2>
            {renderEventsTable(futureEvents)}
          </Stack>
          <Stack direction={"column"}>
            <h2>Past Events</h2>
            {renderEventsTable(pastEvents, false)}
          </Stack>
        </Stack>
        </>
      )}
    </div>
  );
}

export default Home;
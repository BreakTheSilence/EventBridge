import React, {useEffect, useState} from 'react';
import {useNavigate} from "react-router-dom";
import {EventsClient} from "../web-api-client.ts";
import Button from '@mui/material/Button';
import image from '../resources/pilt.jpg'

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
      <div>

      </div>
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
              <Button variant="outlined" onClick={() => handleOpenDetails(event.id)}>Details</Button>
            </td>
            {includeDelete && (
              <td>
                <Button variant="outlined" color="error" onClick={() => handleDeleteEvent(event.id)}>Delete</Button>
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

  function handleAddEvent() {
    navigate("/create-event")
  }

  return (
    <div>
      <div className="text-image-container">
        <div className="text-area">
          <p>
            Sed nec elit vestibulum, tincidunt orci et, sagittis ex. Vestibulum rutrum neque suscipit ante
            mattis maximus. Nulla non sapien viverra, lobortis lorem non, accumsan metus.
          </p>
        </div>
        <div className="image-area">
          <img
            src={image}
            alt="Park bench"
          />
        </div>
      </div>
      {loading ? <p><em>Loading...</em></p> : (
        <>
          <div className="container">
            <div className="events-container">
              <div className="future-events">
                <div className="events-table-header-div">
                  <h2 className="events-table-header">Tulevased üritused</h2>
                </div>
                {renderEventsTable(futureEvents)}
                <Button onClick={handleAddEvent}>LISA ÜRITUS</Button>
              </div>
              <div className="past-events">
                <div className="events-table-header-div">
                  <h2 className="events-table-header">Toimunud üritused</h2>
                </div>
                {renderEventsTable(pastEvents, false)}
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default Home;
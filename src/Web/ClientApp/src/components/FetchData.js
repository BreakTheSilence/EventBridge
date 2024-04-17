import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { EventsClient, AddEventCommand } from '../web-api-client.ts';

function FetchData() {
  const [forecasts, setForecasts] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const populateWeatherData = async () => {
      let eventsClient = new EventsClient();
      const data = await eventsClient.getEventsWithPagination(1, 20);
      setForecasts(data.items);
      setLoading(false);
    };
    populateWeatherData();
  }, []);

  const testEventCreate = async () => {
    const newEvent: AddEventCommand = {
      name: "Spring Festival",
      date: "2024-05-01T00:00:00.000Z",
      location: "Central Park",
      description: "Annual community gathering with food, music, and games."
    };
    console.log(newEvent.name)
    let eventsClient = new EventsClient();
    let eventId = await eventsClient.createEvent(newEvent);
    console.log(eventId)
    navigate(0);
    console.log("navigated")
  };

  const renderForecastsTable = (forecasts) => (
    <div>
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Date</th>
          <th>Location</th>
        </tr>
        </thead>
        <tbody>
        {forecasts.map(forecast => (
          <tr key={forecast.id}>
            <td>{forecast.id}</td>
            <td>{forecast.name}</td>
            <td>{forecast.date.toLocaleDateString()}</td>
            <td>{forecast.location}</td>
          </tr>
        ))}
        </tbody>
      </table>
    </div>
  );

  let contents = loading
    ? <p><em>Loading...</em></p>
    : renderForecastsTable(forecasts);

  return (
    <div>
      <h1 id="tableLabel">Weather forecast</h1>
      <p>This component demonstrates fetching data from the server.</p>
      <button onClick={testEventCreate}>Create Event</button>
      {contents}
    </div>
  );
}

export default FetchData;

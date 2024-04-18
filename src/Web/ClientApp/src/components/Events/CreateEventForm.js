import React, { useState } from 'react';
import { EventsClient, AddEventCommand } from '../../web-api-client.ts';
import { useNavigate } from 'react-router-dom';

function EventForm() {
  const [name, setName] = useState('');
  const [date, setDate] = useState('');
  const [location, setLocation] = useState('');
  const [description, setDescription] = useState('');
  const navigate = useNavigate();

  
  const handleSubmit = async (event) => {
    event.preventDefault();
    console.log('Submitted:', { name, date, location, description });

    const newEvent: AddEventCommand = {
      name: name,
      date: date,
      location: location,
      description: description
    };
    let eventsClient = new EventsClient();
    await eventsClient.createEvent(newEvent);
    navigate("/")
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Name:</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Date:</label>
        <input
          type="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Location:</label>
        <input
          type="text"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
        />
      </div>
      <div>
        <label>Description:</label>
        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>
      <div>
        <button type="submit">Submit</button>
      </div>
    </form>
  );
}
export default EventForm;
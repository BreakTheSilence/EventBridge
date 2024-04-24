import React, {useEffect, useState} from 'react';
import {useParams} from 'react-router-dom';
import {EventsClient} from '../../web-api-client.ts';
import ParticipantsList from '../Participants/ParticipantsList';
import NewParticipantForm from '../EventParticipants/NewParticipantForm';
import '../../custom.css'
import TitleHeader from "../TitleHeader";

function EventDetails() {
  const {eventId} = useParams();
  const [event, setEvent] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchEvent = async () => {
      setLoading(true);
      try {
        let eventsClient = new EventsClient();
        const data = await eventsClient.getEventWithParticipants(eventId)
        setEvent(data);
        setLoading(false);
      } catch (err) {
        setError(err.toString());
        setLoading(false);
      }
    };

    fetchEvent();
  }, [eventId]);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error loading event: {error}</p>;
  }

  return (
    <div className="event-details-container">
      <TitleHeader pageTitle={"Event Details"}/>
      {event ? (
        <div>
          <h2>{event.name}</h2>
          <p>Date: {new Date(event.date).toLocaleDateString()}</p>
          <p>Location: {event.location}</p>
          <p style={{marginBottom: 50}}>Description: {event.description}</p>
          <div className="event-details-participants">
            <ParticipantsList participants={event.eventParticipants} eventId={event.id}/>
            <NewParticipantForm style={{marginTop: 50}} eventId={event.id}/>
          </div>
        </div>
      ) : (
        <p>Event not found</p>
      )}
    </div>
  );
}

export default EventDetails;
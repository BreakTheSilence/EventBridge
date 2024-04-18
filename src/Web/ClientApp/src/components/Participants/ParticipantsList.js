import React, { useState } from 'react';
import {EventParticipantsClient} from "../../web-api-client.ts";

function ParticipantsList({ participants, eventId }) {
  const [participantsList, setParticipants] = useState(participants);


  function handleEdit(participantId) {
    console.log(`Edit participant with ID: ${participantId}`);
  }

  const handleDelete = async (participantId, eventId) => {
    console.log(`Delete participant with ID: ${participantId}`);
    let eventParticipantsClient = new EventParticipantsClient();
    await eventParticipantsClient.deleteEventParticipant(eventId, participantId)
    const updatedParticipants = participantsList.filter(participant => participant.id !== participantId)
    setParticipants(updatedParticipants)
  }
const renderList = (participantsList) => {
  return (
    <div>
      <h2>Participants list</h2>
      <ul>
        {participantsList.map((participant) => (
          <li key={participant.id}>
            {participant.type === 0 ? `${participant.firstName} ${participant.lastName}` : participant.name}
            <span> - ID code: {participant.idCode}</span>
            <button onClick={() => handleEdit(participant.id, eventId)}>Edit</button>
            <button onClick={() => handleDelete(participant.id, eventId)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
 
return(
  <div>{renderList(participantsList)}</div>
)
  
}

export default ParticipantsList;
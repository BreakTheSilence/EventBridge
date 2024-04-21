import React, {useState} from 'react';
import {EventParticipantsClient} from "../../web-api-client.ts";
import {useNavigate} from "react-router-dom";
import '../../custom.css'
import Button from "@mui/material/Button";

function ParticipantsList({participants, eventId}) {
  const navigate = useNavigate();
  const [participantsList, setParticipants] = useState(participants);


  function handleEdit(participantId) {
    console.log(`Edit participant with ID: ${participantId}`);
    navigate(`/participant-details/${eventId}/${participantId}`)
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
      <div className="participants-container">
        <h2>Participants:</h2>
        <ol>
          {participantsList.map((participant, index) => (
            <li key={participant.id}>
              <span>{index + 1}. </span>
              <span>
          {participant.type === 0 ? `${participant.firstName} ${participant.lastName}` : participant.name}
        </span>
              <span className="id-code">{participant.idCode}</span>
              <Button style={{marginRight: 10}} variant="contained" color="warning"
                      onClick={() => handleEdit(participant.id, eventId)}>Edit</Button>
              <Button variant="outlined" color="error"
                      onClick={() => handleDelete(participant.id, eventId)}>Delete</Button>
            </li>
          ))}
        </ol>
      </div>
    );
  }

  return (
    <div>{renderList(participantsList)}</div>
  )

}

export default ParticipantsList;
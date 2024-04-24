import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {EventParticipantsClient} from "../../web-api-client.ts";
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import EditParticipantDetails from "./EditParticipantDetails";
import EditParticipationDetails from "../EventParticipants/EditParticipationDetails";
import TitleHeader from "../TitleHeader";

function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`tabpanel-${index}`}
      aria-labelledby={`tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

function a11yProps(index) {
  return {
    id: `tab-${index}`,
    'aria-controls': `tabpanel-${index}`,
  };
}

function ParticipantDetails() {
  const { participantId, eventId } = useParams();
  const [eventParticipant, setEventParticipant] = useState(null);
  const [value, setValue] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchEvent = async () => {
      setLoading(true);
      try {
        let eventsClient = new EventParticipantsClient();
        const data = await eventsClient.getEventParticipant(eventId, participantId);
        setEventParticipant({ ...data, participant: data.participant || {} });
        setLoading(false);
      } catch (err) {
        setError(err.toString());
        setLoading(false);
      }
    };

    fetchEvent();
  }, [eventId, participantId]);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error loading event: {error}</p>;
  }

  return (
    <div className="event-details-container">
      <TitleHeader pageTitle={"Details"}/>
    <Box sx={{ width: '100%' }}>
      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={value} onChange={handleChange} aria-label="basic tabs example">
          <Tab label="Participation Details" {...a11yProps(0)} />
          <Tab label="Participant Details" {...a11yProps(1)} />
        </Tabs>
      </Box>
      <TabPanel value={value} index={0}>
       <EditParticipationDetails eventParticipantDto={eventParticipant}/>
      </TabPanel>
      <TabPanel value={value} index={1}>
        <EditParticipantDetails participantDto={eventParticipant.participant}/>
      </TabPanel>
    </Box>
    </div>
  );
}

export default ParticipantDetails;

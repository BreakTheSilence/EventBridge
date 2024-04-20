import  Home  from "./components/Home";
import  CreateEvent  from "./components/Events/CreateEvent";
import  EventDetails  from "./components/Events/EventDetails";
import  ParticipantDetails  from "./components/Participants/ParticipantDetails";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/create-event',
    element: <CreateEvent />
  },
  {
    path: '/event-details/:eventId',
    element: <EventDetails />
  },
  {
    path: '/participant-details/:eventId/:participantId',
    element: <ParticipantDetails />
  }
];

export default AppRoutes;

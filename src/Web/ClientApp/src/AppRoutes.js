import  Home  from "./components/Home";
import  CreateEvent  from "./components/Events/CreateEvent";
import  EventDetails  from "./components/Events/EventDetails";

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
  }
];

export default AppRoutes;

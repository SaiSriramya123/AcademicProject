import React from 'react';

import { Navbar, Container, Badge } from 'react-bootstrap';

const Header = ({ instId, phone }) => {

  return (
<Navbar bg="white" className="shadow-sm mb-4 py-3 border-bottom">
<Container fluid className="px-4">
<Navbar.Brand className="fw-bold fs-3 text-primary">EDU TRACK</Navbar.Brand>
<div className="ms-auto d-flex align-items-center gap-3">
<div className="text-end me-3 d-none d-md-block">
<small className="text-muted d-block">Instructor Dashboard</small>
<span className="fw-bold text-dark">{phone}</span>
</div>
<Badge bg="primary" pill className="px-3 py-2 fs-6 shadow-sm">

            ID: {instId}
</Badge>
</div>
</Container>
</Navbar>

  );

};

export default Header;
 
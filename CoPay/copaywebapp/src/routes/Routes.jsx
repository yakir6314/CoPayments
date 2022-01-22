import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import {HomePage} from '../pages/HomePage';

const RoutesApp = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage/>}>
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default RoutesApp;

import React from "react";
import {BrowserRouter as Router, Link, Route, Routes} from "react-router-dom";
import AdminPage from "../AdminComponent/Admin";
import Registration from "../Registration/Registration";
import Authorization from "../Authorization/Authorization";
import 'bootstrap/dist/css/bootstrap.css';
import NotFound from "../404/NotFound";
import Files from "../File/Files";
import Profile from "../Profile/Profile";
import './Header.css';
import ChangePass from "../Profile/ChangePass";

function Header() {
    const signOut = () => {
        localStorage.clear()
        window.location.href = "http://localhost:3000/signin";
    }
    return (
        <>
            <Router>
            <nav style={{paddingBottom:'1%'}}>
                <ul className="ulHeader">
                    {localStorage.hasOwnProperty('UserId') ? (
                        <li className="liHeader">
                            <Link style={{ textDecoration: 'none' }} className="" to="/files">
                                Files
                            </Link>
                        </li>
                    ) : (
                        <li className="liHeader">
                            <Link style={{ textDecoration: 'none' }} className="" to="/signin">
                                Sign in
                            </Link>
                        </li>
                    )}
                    {localStorage.getItem('roles')?.includes("Administrator") &&

                        <li className="liHeader">
                            <Link style={{ textDecoration: 'none' }}  className="" to="/admin">
                                Admin menu
                            </Link>
                        </li>

                    }
                    {!localStorage.hasOwnProperty('UserId') &&
                        <li className="liHeader">
                            <Link style={{ textDecoration: 'none' }} className="" to="/signup">
                                Sign up
                            </Link>
                        </li>
                    }
                    {localStorage.hasOwnProperty('UserId') &&
                    <li style={{float:'right'}}>
                        <Link style={{ textDecoration: 'none' }} className="image" to="/profile">
                            <img src="https://cdn-icons-png.flaticon.com/512/20/20079.png"
                            height="52"/>
                        </Link>
                    </li>
                    }
                    {localStorage.hasOwnProperty('UserId') &&
                        <li style={{float:'right'}} className="liHeader">
                            <Link style={{ textDecoration: 'none'}} className="btn-outline-danger" to="/signin" onClick={signOut}>
                                Sign out
                            </Link>
                        </li>}
                    </ul>
            </nav>
                <Routes>
                    <Route path="/files" element={<Files/>}/>
                    <Route path="/notfound" element={<NotFound/>}/>
                    <Route path="/admin" element={<AdminPage/>}/>
                    <Route path="/signup" element={<Registration/>} />
                    <Route path="/signin" element={<Authorization/>} />
                    <Route path="/profile" element={<Profile/>} />
                    <Route path="/profile/changepass" element={<ChangePass/>} />
                </Routes>
            </Router>
        </>
    );
}
export default Header;
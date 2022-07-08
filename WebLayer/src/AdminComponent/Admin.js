import React, {useState, useEffect} from "react";

import EditUser from "./EditUser";
import {variables} from "../Variables/Variables";
import '../FileComponent/File.css';
const AdminPage = (props) => {


    const [showEditBtn, setShowEditBtn] = useState(false);

    const [users, setUsers] = useState([]);

    const handleCloseEditButton = () => setShowEditBtn(false);

    const handleShowEditButton = () => setShowEditBtn(true);


    const [currentUser, setCurrentUser] = useState();


    const [refresh, setRefresh] = useState(false);

    const refreshPage = () => {
        setRefresh(true)
    }

    useEffect(() => {
        fetch(variables.API_URL + 'Authentication')
            .then(response => response.json())
            .then(data => {
                setUsers(data);
                setRefresh(false)
            })
    }, [refresh])


    const updateUser = (email, firstname, lastname, nickname) => {
        fetch(variables.API_URL + "Authentication/" + currentUser.userId, {
            method: 'PATCH',
            body: JSON.stringify([
                {
                    op:"replace",
                    path: "Email",
                    value: email
                },
                {
                    op:"replace",
                    path: "FirstName",
                    value: firstname
                },
                {
                    op:"replace",
                    path:"LastName",
                    value: lastname
                }
                ,
                {
                    op:"replace",
                    path:"UserName",
                    value: nickname
                }
            ]),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
            },
        })
            .then((response) => {
                console.log(response);
                refreshPage();
            })
    }
    const editUser = (user) => {
        setCurrentUser(user)
        if (user !== undefined) {
            handleShowEditButton()
        }
    }

    const deleteUser = (user) => {
        if (window.confirm('Are you sure?')) {
            fetch(variables.API_URL + 'Authentication/' + user.userId, {
                method: 'DELETE',
                headers: {
                    'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
                    'Content-Type': 'application/json'
                }
            })

                .then(res => res.json())
                .then((result) => {
                    refreshPage();
                }, (error) => {
                    alert('Failed');
                })
        }
    }
    const makeAnAdmin = (user) => {
        fetch(variables.API_URL + 'Authentication/addrole/Administrator', {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user')),
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user.email)
        })
            .then(res => res.json())
            .then((result) => {
                refreshPage();
            })
    }
    console.log(users);
    if(!localStorage.getItem('roles')?.includes("Administrator")){
        window.location.href = "http://localhost:3000/notfound";
    }
    else if (users.length !== 0)
        return (
            <div>
                <table className="content-table">
                    <thead>
                    <tr>
                        <th>
                            UserId
                        </th>

                        <th>
                            Email
                        </th>

                        <th>
                            FirstName
                        </th>

                        <th>
                            SecondName
                        </th>

                        <th>
                            Role
                        </th>
                    </tr>

                    </thead>
                    <tbody>
                    {users.map(user =>
                        <tr key={user.userId}>
                            <td>{users?.indexOf(user) + 1}</td>
                            <td>{user.email}</td>
                            <td>{user.firstName}</td>
                            <td>{user.lastName}</td>
                            <td>{user.roles?.join(",")}</td>
                            <td>
                                {!user.roles?.includes("Administrator") &&
                                <button type="button"
                                        className="btn btn-outline-info m-lg-1"
                                        onClick={editUser.bind(this, user)}>Edit user
                                </button>
                                }
                                {!user.roles?.includes("Administrator") &&
                                <button type="button"
                                        className="btn btn-outline-danger m-lg-1"
                                        onClick={deleteUser.bind(this, user)}>Delete user
                                </button>}
                                {!user.roles?.includes("Administrator") &&
                                    <button type="button"
                                            className="btn btn-outline-success m-lg-1"
                                            onClick={makeAnAdmin.bind(this, user)}>Make an admin
                                    </button>
                                }
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
                <EditUser
                    handleShowEditButton={handleShowEditButton}
                    handleCloseEditButton={handleCloseEditButton}
                    showEditBtn={showEditBtn}
                    refreshPage={refreshPage}
                    users={users}
                    updateUser={updateUser}
                    currentuser={currentUser}

                />
            </div>
        )
    else return (<button className="btn btn-primary" disabled>
        <span className="spinner-border spinner-border-sm"></span>
        Loading..
    </button>)


}
export default AdminPage;
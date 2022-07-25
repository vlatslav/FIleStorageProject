import React, {useEffect, useState} from "react";
import './Profile.css';
import {variables} from "../Variables/Variables";
import EditUser from "../AdminComponent/EditUser";
import ChangePass from "./ChangePass";
const Profile = () => {
    const [currentUser,setCurrentUser] = useState({});
    const [showEditBtn, setShowEditBtn] = useState(false);
    const [showChangeBtn, setShowChangeBtn] = useState(false);
    const [users, setUsers] = useState([]);

    const handleCloseEditButton = () => setShowEditBtn(false);
    const handleCloseChangeButton = () => setShowChangeBtn(false);

    const handleShowEditButton = () => setShowEditBtn(true);
    const handleShowChangeButton = () => setShowChangeBtn(true);

    const [refresh, setRefresh] = useState(false);
    const refreshPage = () => {
        setRefresh(true);
    }
    useEffect(() => {
        fetch(variables.API_URL + 'Authentication')
            .then(response => {
                if(!response.ok)
                {
                    throw new Error();
                }
                return response.json();
            })
            .then(data => {
                const id = localStorage.getItem('UserId');
                const user = data?.find(user => user.userId === id);
                setUsers(data);
                setCurrentUser(user);
                setRefresh(false);
            }).catch(err => {
                alert("error");
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
                if(!response.ok){
                    throw new Error();
                }
                console.log(response);
                refreshPage();
            }).catch(err => {
                alert("Error");
        })
    }
    return(
      <>
          <div className="wrapper">
          <div className="leftmenu">
              <div style={{paddingBottom: '2%'}}>
                  <button style={{width: '90%'}} type="button"
                          className="btn btn-primary btn-outline-info"
                          onClick={handleShowChangeButton}>Change Password
                  </button>
              </div>
              <div>
              <button style={{width: '90%'}} type="button"
                      className="btn btn-primary btn-outline-info"
                      onClick={handleShowEditButton}>Edit user
              </button>
              </div>
          </div>
              <div>
                  <div><h3 className="text-primary">Username: {currentUser.userName}</h3></div>
                  <div><h3 className="text-primary">Email: {currentUser.email}</h3></div>
                  <div><h3 className="text-primary">Firstname: {currentUser.firstName}</h3></div>
                  <div><h3 className="text-primary">Lastname: {currentUser.lastName}</h3></div>
              </div>
          </div>
          <EditUser
              handleShowEditButton={handleShowEditButton}
              handleCloseEditButton={handleCloseEditButton}
              showEditBtn={showEditBtn}
              refreshPage={refreshPage}
              users={users}
              updateUser={updateUser}
              currentuser={currentUser}
          />
          <ChangePass
              handleShowChangeButton={handleShowChangeButton}
              handleCloseChangeButton={handleCloseChangeButton}
              showChangeBtn={showChangeBtn}
              refreshPage={refreshPage}
          />
      </>
    );
}
export default Profile;
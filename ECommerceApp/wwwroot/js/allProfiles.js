const SearchInput = document.querySelector("#SearchInput");
const ProfileContainer = document.querySelector("#ProfileContainer");
const profiles = Array.from(ProfileContainer.querySelectorAll("a"));
const noProfileFoundMessage = document.querySelector("#NoProfileFoundMessage");

function search() {
    const searchValue = SearchInput.value.toLowerCase();

    if (searchValue === "") {
        profiles.forEach(profile => {
            profile.style.visibility = "visible";
        });
        noProfileFoundMessage.classList.add("hidden"); 
        return;
    }

    let hasResults = false;

    profiles.forEach(profile => {
        const profileName = profile.getAttribute("data-profile-name").toLowerCase();
        if (profileName.includes(searchValue)) {
            profile.style.visibility = "visible";
            hasResults = true; 
        } else {
            profile.style.visibility = "hidden";
        }
    });

    if (hasResults) {
        noProfileFoundMessage.classList.add("hidden");
    } else {
        noProfileFoundMessage.classList.remove("hidden");
    }
}

SearchInput.addEventListener("input", search);

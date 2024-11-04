const SearchInput = document.querySelector("#SearchInput");
const ProfileContainer = document.querySelector("#ProfileContainer");
const profiles = Array.from(ProfileContainer.querySelectorAll("a"));
const noProfileFoundMessage = document.querySelector("#NoProfileFoundMessage");

function search() {
    const searchValue = SearchInput.value.toLowerCase();

    profiles.forEach(profile => {
        const profileName = profile.getAttribute("data-profile-name").toLowerCase();
        if (profileName.includes(searchValue)) {
            profile.style.display = "flex";
        } else {
            profile.style.display = "none";
        }
    });

}

SearchInput.addEventListener("input", search);
